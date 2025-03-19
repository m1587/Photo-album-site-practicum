import { useContext } from 'react';
import { Avatar, Box, Typography } from '@mui/material';
import { UserContext } from '../context/UserContext';

export const UserName = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('YourComponent must be used within a UserProvider');
  }
  const { state } = context;

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: '4px', zIndex: 1300, marginTop: 2 }}>
      <Avatar sx={{ bgcolor: '#e1c16e' }}>
        {state.name.charAt(0).toUpperCase()}
      </Avatar>
      <Typography variant="h6" sx={{
        color: 'white',
        fontFamily: 'Roboto, sans-serif',
        fontWeight: 'bold',
      }}>{state.name} </Typography>
    </Box>
  );
};



