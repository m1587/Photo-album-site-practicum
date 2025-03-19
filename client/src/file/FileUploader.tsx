
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, IconButton, LinearProgress, List, ListItem, ListItemText, TextField, Typography } from '@mui/material';
import { useContext } from 'react';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import FolderIcon from '@mui/icons-material/Folder';
import LabelIcon from '@mui/icons-material/Label';
import api from '../axiosConfig';
import { UserContext } from '../context/UserContext';
import axios from 'axios';
// import axios, { AxiosProgressEvent } from 'axios'; לפני
const FileUploader = () => {
    const context = useContext(UserContext);
    if (!context) { throw new Error('Your Component must be used within a UserProvider'); }
  const [file, setFile] = useState<File | null>(null);
  const [progress, setProgress] = useState(0);
  const [uploadedFiles, setUploadedFiles] = useState<string[]>([]);
  const [showFiles, setShowFiles] = useState(false);
  const [newFolder, setNewFolder] = useState("");
  const [newFileName, setNewFileName] = useState("");
  const [tag, setTag] = useState({ key: "", value: "" });
  const navigate = useNavigate();
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };
  const getToken = () => {
    const token = localStorage.getItem("token");
    if (!token) {
      alert("לא נמצא טוקן, נא להתחבר מחדש.");
      navigate('User/login'); // אם יש לך גישה לניווט
      return null;
    }
    return token;
  };
  const handleUpload = async () => {
    if (!file) return;

    try {
      // שלב 1: קבלת Presigned URL מהשרת
      const token = getToken();
      const response = await api.get('/upload/presigned-url', {
        params: { 
          fileName: file.name,
          userId: context.state.id 
         },
        headers: { Authorization: `Bearer ${token}` }, // הוספת הטוקן
      });
      console.log("b");
      
      const presignedUrl = response.data.url;
      // שלב 2: העלאת הקובץ ישירות ל-S3
      await axios.put(presignedUrl, file, {
        // headers: {'Content-Type': file.type,}, 
        // onUploadProgress: (progressEvent:AxiosProgressEvent   ) => {  לפני 
        onUploadProgress: (progressEvent) => {
          const percent = Math.round(
            ((progressEvent.loaded || 0) * 100) / (progressEvent.total || 1)
          );
          setProgress(percent);
        },
      });   
      alert('הקובץ הועלה בהצלחה!');
      fetchUploadedFiles();
    } catch (error) {
      console.error('שגיאה בהעלאה:', error);
    }
  };
  const fetchUploadedFiles = async () => {
    try {
      const token = getToken();
      const response = await api.get<string[]>("upload/list-files", {
        params: { userId: context.state.id },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setUploadedFiles(response.data);
    } catch (error) {
      console.error('שגיאה בהבאת רשימת הקבצים:', error);
    }
  };

  useEffect(() => {
    fetchUploadedFiles(); // קריאה לשליפת הקבצים כשעמוד נטען
  }, []);

  // הצגת רשימת הקבצים כאשר הכפתור נלחץ
  const toggleFilesDisplay = () => {
    setShowFiles((prev) => !prev);
  };
  const handleDelete = async (fileName: string) => {
    try {
      const token = getToken();
      await api.delete(`upload/delete-file?fileName=${fileName}`, {
        params: { userId: context.state.id },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      alert('הקובץ נמחק בהצלחה!');
      fetchUploadedFiles();
    } catch (error) {
      console.error('שגיאה במחיקת הקובץ:', error);
    }
  };
  const handleRename = async (oldFileName: string) => {
    if (!newFileName) return;
    try {
      const token = getToken();
      
      // קריאה לשירות לשנות את שם הקובץ
      await api.put('upload/rename-file', null, {
        params: {
          oldFileName,
          newFileName,
          userId: context.state.id,
        },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
  
      // קריאה להוסיף תיוג אחרי שינוי שם
      await api.put('upload/tag-file', null, {
        params: {
          fileName: newFileName,  // שם הקובץ החדש
          tagKey: 'renamed',      // מפתח התגית
          tagValue: 'true',       // ערך התגית
        },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
      alert("שם הקובץ עודכן בהצלחה!");
      fetchUploadedFiles();
    } catch (error) {
      console.error("שגיאה בשינוי שם:", error);
    }
  };
  
  const handleMove = async (fileName: string) => {
    if (!newFolder) {
      console.log("No new folder provided");
      return;
    }
    console.log(`Moving file: ${fileName} to folder: ${newFolder}`);
    try {
      const token = getToken();
      const response = await api.put(`upload/move-file`, null, {
        params: { fileName, newFolder, userId: context.state.id },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      console.log(response.data); // הוספת לוג לתגובה מהשרת
      alert("הקובץ הועבר בהצלחה!");
      fetchUploadedFiles();
    } catch (error) {
      console.error("שגיאה בהעברה:", error);
    }
  };
  const handleTag = async (fileName: string) => {
    if (!tag.key || !tag.value) return;
    try {
      const token = getToken();
      await api.put(`upload/tag-file`, null, {
        params: { fileName, tagKey: tag.key, tagValue: tag.value , userId: context.state.id },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      alert("תגית נוספה בהצלחה!");
    } catch (error) {
      console.error("שגיאה בהוספת תגית:", error);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <input
        type="file"
        onChange={handleFileChange}
        style={{ marginBottom: '10px' }}
      />
      <Button
        variant="contained"
        color="primary"
        onClick={handleUpload}
        startIcon={<CloudUploadIcon />}
        disabled={!file}
      >
        העלה קובץ
      </Button>
      {progress > 0 && (
        <div style={{ marginTop: '10px' }}>
          <Typography variant="body2">התקדמות: {progress}%</Typography>
          <LinearProgress variant="determinate" value={progress} />
        </div>
      )}
      {/* כפתור להראות או להסתיר את רשימת הקבצים שהועלו */}
      <Button
        variant="outlined"
        color="secondary"
        onClick={toggleFilesDisplay}
        style={{ marginTop: '20px' }}
      >
        {showFiles ? 'הסתר קבצים' : 'הצג קבצים'}
      </Button>

      {/* הצגת הקבצים שהועלו */}
      {showFiles && (
        <div style={{ marginTop: '20px' }}>
          <Typography variant="h6">קבצים שהועלו:</Typography>
          <List>
            {uploadedFiles.length > 0 ? (
              uploadedFiles.map((fileName, index) => (
                <ListItem key={index}>
                  <ListItemText primary={fileName} />

                  <IconButton onClick={() => handleDelete(fileName)} color="error">
                    <DeleteIcon />
                  </IconButton>

                  <TextField label="שם חדש" size="small" onChange={(e) => setNewFileName(e.target.value)} />
                  <IconButton color="primary" onClick={() => handleRename(fileName)}>
                    <EditIcon />
                  </IconButton>

                  <TextField label="תיקייה" size="small" onChange={(e) => setNewFolder(e.target.value)} />
                  <IconButton onClick={() => handleMove(fileName)}>
                    <FolderIcon />
                  </IconButton>

                  <TextField label="תגית" size="small" onChange={(e) => setTag({ ...tag, key: e.target.value })} />
                  <TextField label="ערך" size="small" onChange={(e) => setTag({ ...tag, value: e.target.value })} />
                  <IconButton onClick={() => handleTag(fileName)}>
                    <LabelIcon />
                  </IconButton>

                </ListItem>
              ))
            ) : (
              <Typography variant="body2">לא הועלו קבצים עדיין.</Typography>
            )}
          </List>
        </div>
      )}
    </div>
  );
};

export default FileUploader;