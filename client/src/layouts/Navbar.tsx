import { Link } from 'react-router-dom';
import { AppBar, Toolbar, Button } from '@mui/material';
import HomeIcon from '@mui/icons-material/Home';
import ContactIcon from '@mui/icons-material/Phone';
const Navbar = () => {

  return (
    <AppBar color="transparent" sx={{ bgcolor: "#f9f1a5", boxShadow: 3, height: '11%' }}>
      <Toolbar sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'flex-start', paddingRight: 2 }}>
      <Button
          color="inherit"
          component={Link}
          to='/Contact'
          sx={{ marginTop: 2, color: '#333333', fontWeight: 'bold' }}
        >
          <ContactIcon fontSize="large" />
        </Button>
        <Button
          color="inherit"
          component={Link}
          to='/'
          sx={{ marginTop: 2, color: '#333333', fontWeight: 'bold' }}
        >
          <HomeIcon fontSize="large" />
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
