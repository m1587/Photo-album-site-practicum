
import { useState } from 'react';
import Box from '@mui/material/Box'; // ייבוא רכיב Box מ-MUI
import { Login } from '../user/Login';
import { Register } from '../user/Registration';
import { UserName } from '../user/UserName';
import FileUploader from '../file/FileUploader';
import Navbar from './Navbar';

const AppLayout = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const handleLoginSuccess = () => {
    setIsLoggedIn(true);
  };
  return (
    <>
      <Box
        sx={{
          backgroundImage: 'url(public/images/1.jpg)',
          backgroundSize: 'contain',
          minHeight: '100vh',
          overflow: 'hidden',
        }}
      >
        <Navbar />
        <Box sx={{
          position: 'fixed',
          top: 10,
          left: 10,
          borderRadius: 1,
          zIndex: 1100,
        }}>
          {isLoggedIn ? (
            <>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                <UserName />
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                <FileUploader />
              </Box>
            </>
          ) : (
            <>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                <Login onLoginSuccess={handleLoginSuccess} />
                <Register />
              </Box>
            </>
          )}
        </Box>
      </Box>
    </>
  );
};

export default AppLayout;
