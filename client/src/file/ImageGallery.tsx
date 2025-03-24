import {  useState, useContext } from 'react';
import { Button, IconButton, List, ListItem, Typography } from '@mui/material';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import api from '../axiosConfig';
import { UserContext } from '../context/UserContext';

interface ImageGalleryProps {
  uploadedFiles: { fileName: string, url: string }[];
}

const ImageGallery: React.FC<ImageGalleryProps> = ({ uploadedFiles }) => {
  const context = useContext(UserContext);
  if (!context) { throw new Error('Your Component must be used within a UserProvider'); }
  
  const [likes, setLikes] = useState<{ [key: string]: number }>({});
  const [showFiles, setShowFiles] = useState(false);

  const getToken = () => {
    return localStorage.getItem("token");
  };

  const fetchAllVotesCount = async () => {
    const token = getToken();
    if (!token) return;

    try {
      const updatedLikes: { [key: string]: number } = {};
      for (const file of uploadedFiles) {
        if (!file?.fileName) continue;
        const response = await api.get(`Image/Name`, {
          params: { imageName: file.fileName },
          headers: { Authorization: `Bearer ${token}` },
        });
        const imageId = response.data?.id;
        if (!imageId) continue;
        const votesResponse = await api.get("Vote/Count", {
          params: { imageId },
          headers: { Authorization: `Bearer ${token}` },
        });
        updatedLikes[file.fileName] = votesResponse.data?.voteCount || 0;
      }
      setLikes(updatedLikes);
    } catch (error) {
      console.error("שגיאה בשליפת מספר הלייקים:", error);
    }
  };

  const handleLike = async (imageName: string) => {
    const token = getToken();
    if (!token) return;
    try {
      setLikes((prevLikes) => ({
        ...prevLikes,
        [imageName]: (prevLikes[imageName] || 0) + 1,
      }));
      const response = await api.get(`Image/Name`, {
        params: { imageName },
        headers: { Authorization: `Bearer ${token}` },
      });
      const imageId = response.data?.id;
      if (!imageId) return;
      await api.post("Vote", {
        userId: context.state.id,
        imageId: imageId,
      }, {
        headers: { Authorization: `Bearer ${token}` },
      });
      await fetchAllVotesCount();
    } catch (error) {
      console.error("Error during vote:", error);
    }
  };

  return (
    <div>
      <Button variant="outlined" color="secondary" onClick={() => setShowFiles(!showFiles)}>
        {showFiles ? 'הסתר תמונות' : 'הצג תמונות'}
      </Button>
      {showFiles && (
        <List>
          {uploadedFiles.length > 0 ? (
            uploadedFiles.map((file, index) => (
              file?.fileName ? (
                <ListItem key={index}>
                  <img
                    src={`https://login-user-bucket-testpnoren.s3.us-east-1.amazonaws.com/${file?.fileName}`}
                    style={{ width: '150px', height: '150px', objectFit: 'cover', marginRight: '10px' }}
                  />
                  <IconButton onClick={() => handleLike(file.fileName)} color="primary">
                    <ThumbUpIcon />
                  </IconButton>
                  <Typography>{likes[file.fileName] || 0}</Typography>
                </ListItem>
              ) : null
            ))
          ) : (
            <Typography variant="body2">לא הועלו תמונות עדיין.</Typography>
          )}
        </List>
      )}
    </div>
  );
};

export default ImageGallery;
