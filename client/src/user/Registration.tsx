import React, { useState, useContext } from 'react';
import { TextField, Button, Modal, Box } from '@mui/material';
import axios from 'axios';
import ErrorSnackbar from '../Error';
import { UserContext } from '../context/UserContext';
export const Register = () => {
  const context = useContext(UserContext);
  if (!context) { throw new Error('Your Component must be used within a UserProvider'); }
  const { dispatch } = context;
  const [open, setOpen] = useState(false);
  const [formData, setFormData] = useState({ name: '', email: '', password: ''});
  const [error, setError] = useState<any>(null);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errors, setErrors] = useState<any>({ name: '', email: '', password: '' });
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
    setErrors({ ...errors, [e.target.name]: '' });
  };
  const handleSave = async () => {
    const newErrors: any = {};
    if (!formData.name) newErrors.name = 'First name is required';
    if (!formData.email) newErrors.email = 'Email is required';
    else if (!/^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/.test(formData.email)) {
      newErrors.email = 'Please enter a valid email address';}
    if (!formData.password) newErrors.password = 'Password is required';
    setErrors(newErrors);
    if (Object.keys(newErrors).length > 0) { return; }
    try {
      console.log(formData);
      const response = await axios.post('https://localhost:7123/api/User', {
        email: formData.email,
        password: formData.password,
        name: formData.name
      });
      console.log(response.data); // הוספת לוג לבדיקת המבנה של התשובה
      // const user:any = response.data; 
      // dispatch({ type: 'CREATE_USER', payload: user});
      dispatch({ type: 'CREATE_USER', payload:  response.data});

      alert('Registration successful!');
      setFormData({ name: '', email: '', password: '' });
      setOpen(false);
    } catch (error: any) {
      setError(error);
      setOpenSnackbar(true);
    }};
  return (<>
      <Button onClick={() => setOpen(true)}
       sx={{ bgcolor: 'rgba(255, 255, 255, 0.8)', color: '#333333', zIndex: 1300, marginTop: 2, fontWeight: 'bold' }}>Register</Button>
      <Modal open={open} onClose={() => setOpen(false)}>
        <Box 
         sx={{ position: 'absolute', top: '50%', left: '50%',
          transform: 'translate(-50%, -50%)', padding: 2, width: 300, backgroundColor: 'white', borderRadius: 1, boxShadow: 24, zIndex: 1300, }}>
          <TextField
            label="Name" name="name" value={formData.name}
            onChange={handleChange} fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.name}
            helperText={errors.name} />
          <TextField
            label="Email" name="email" value={formData.email}
            onChange={handleChange}
            type="email" fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.email}
            helperText={errors.email} />
          <TextField
            label="Password" name="password" value={formData.password}
            onChange={handleChange}
            type="password" fullWidth sx={{ marginBottom: 2 }}
            error={!!errors.password}
            helperText={errors.password} />
          <Button onClick={handleSave} sx={{ width: '100%', backgroundColor: '#C4A36D', color: 'white', padding: 1 }}>Register</Button>
        </Box>
      </Modal>
      <ErrorSnackbar error={error} open={openSnackbar} onClose={() => setOpenSnackbar(false)} />
    </>);};

