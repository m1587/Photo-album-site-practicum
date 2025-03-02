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
  const [formData, setFormData] = useState({ firstName: '', lastName: '', email: '', address: '', phone: '', password: '', });
  const [error, setError] = useState<any>(null);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errors, setErrors] = useState<any>({ firstName: '', lastName: '', email: '', address: '', phone: '', password: '', });
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
    setErrors({ ...errors, [e.target.name]: '' });
  };
  const handleSave = async () => {
    const newErrors: any = {};
    if (!formData.firstName) newErrors.firstName = 'First name is required';
    if (!formData.lastName) newErrors.lastName = 'Last name is required';
    if (!formData.email) newErrors.email = 'Email is required';
    else if (!/^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/.test(formData.email)) {
      newErrors.email = 'Please enter a valid email address';}
    if (!formData.address) newErrors.address = 'Address is required';
    if (!formData.phone) {
      newErrors.phone = 'Phone is required';
    } else if (!/^\d{2,3}-\d{7}$/.test(formData.phone) && !/^\d{10}$/.test(formData.phone)) {
      newErrors.phone = 'Please enter a valid phone number (e.g. 03-1234567 or 050-1234567)';}
    if (!formData.password) newErrors.password = 'Password is required';
    setErrors(newErrors);
    if (Object.keys(newErrors).length > 0) { return; }
    try {
      console.log(formData);
      const res = await axios.post('http://localhost:3000/api/user/register', {
        email: formData.email,
        password: formData.password,
        firstName: formData.firstName,
        lastName: formData.lastName,
        address: formData.address,
        phone: formData.phone,
      });
      dispatch({ type: 'CREATE_USER', payload: res.data.user });
      alert('Registration successful!');
      setFormData({ firstName: '', lastName: '', email: '', address: '', phone: '', password: '', });
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
            label="First Name" name="firstName" value={formData.firstName}
            onChange={handleChange} fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.firstName}
            helperText={errors.firstName} />
          <TextField
            label="Last Name" name="lastName" value={formData.lastName}
            onChange={handleChange} fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.lastName}
            helperText={errors.lastName} />
          <TextField
            label="Email" name="email" value={formData.email}
            onChange={handleChange}
            type="email" fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.email}
            helperText={errors.email} />
          <TextField
            label="Address" name="address" value={formData.address}
            onChange={handleChange} fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.address}
            helperText={errors.address} />
          <TextField
            label="Phone" name="phone" value={formData.phone}
            onChange={handleChange} fullWidth
            sx={{ marginBottom: 2 }}
            error={!!errors.phone}
            helperText={errors.phone} />
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

