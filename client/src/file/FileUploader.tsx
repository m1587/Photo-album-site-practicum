
import React, { useEffect, useState, useContext } from 'react';
import { Button, LinearProgress } from '@mui/material';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';
import api from '../axiosConfig';
import { UserContext } from '../context/UserContext';
import ImageGallery from './ImageGallery';
import axios from 'axios';

const FileUploader = () => {
  const context = useContext(UserContext);
  if (!context) throw new Error('Your Component must be used within a UserProvider');

  const [file, setFile] = useState<File | null>(null);
  const [progress, setProgress] = useState(0);
  const [uploadedFiles, setUploadedFiles] = useState<{ fileName: string, url: string }[]>([]);
  const [showFiles, setShowFiles] = useState(false);

  const getToken = () => localStorage.getItem('token');

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };
  const handleUpload = async () => {
    if (!file) return;

    try {
      // שלב 1: קבלת Presigned URL מהשרת
      const token = getToken();
      const response = await api.get('/upload/presigned-url', {
        params: {
          fileName: file.name,
        },
        headers: { Authorization: `Bearer ${token}` }, // הוספת הטוקן
      });
      const presignedUrl = response.data.url;
      // שלב 2: העלאת הקובץ ישירות ל-S3
      await axios.put(presignedUrl, file, {
        headers: { 'Content-Type': file.type, },
        onUploadProgress: (progressEvent) => {
          const percent = Math.round(
            ((progressEvent.loaded || 0) * 100) / (progressEvent.total || 1)
          );
          setProgress(percent);
        },
      });
      ///
      const imageData = {
        UserId: context.state.id,
        // challengeId: 1, // כאן תכניס את ה-Id של האתגר המתאים
        ImageURL: `https://login-user-bucket-testpnoren.s3.us-east-1.amazonaws.com/${file.name}`, // ה-URL המלא
        Caption: 'תיאור התמונה כאן', // אפשר להוסיף תיאור או כותרת
      };
      console.log("imageData being sent:", imageData);
      try {
        await api.post('Image', imageData, {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
          }
        });
      } catch (error) {
        console.log('שגיאה בהעלאת התמונה:', error);
      }
      alert('הקובץ הועלה בהצלחה!');
      fetchUploadedFiles();
      // fetchVotesCount(uploadedFiles);
    } catch (error) {
      console.error('שגיאה בהעלאה:', error);
    }
  };
  const fetchUploadedFiles = async () => {
    try {
      const token = getToken();
      const response = await api.get("upload/list-files", {
        headers: { Authorization: `Bearer ${token}` },
      });

      // הפקת Presigned URL לכל קובץ
      const signedUrls = await Promise.all(
        response.data.map(async (fileName: string) => {
          const presignedResponse = await api.get("/upload/presigned-url", {
            params: { fileName },
            headers: { Authorization: `Bearer ${token}` },
          });
          return { fileName,
             url: presignedResponse.data.url
            };
        })
      );
      setUploadedFiles(signedUrls);
    } catch (error) {
      console.error("שגיאה בהבאת רשימת הקבצים:", error);
    }
  };
  useEffect(() => {
    fetchUploadedFiles();
  }, []);
  return (
    <div style={{ padding: '20px' }}>
      <input type="file" onChange={handleFileChange} style={{ marginBottom: '10px' }} />
      <Button variant="contained" color="primary" onClick={handleUpload} startIcon={<CloudUploadIcon />} disabled={!file}>
        העלה קובץ
      </Button>
      {progress > 0 && <LinearProgress variant="determinate" value={progress} style={{ marginTop: '10px' }} />}
      <Button variant="outlined" color="secondary" onClick={() => setShowFiles((prev) => !prev)} style={{ marginTop: '20px' }}>
        {showFiles ? 'הסתר תמונות' : 'הצג תמונות'}
      </Button>
      {showFiles && <ImageGallery uploadedFiles={uploadedFiles} />}
    </div>
  );
};
export default FileUploader;

