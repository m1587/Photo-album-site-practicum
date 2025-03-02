import React, { createContext, useReducer, ReactNode } from 'react';
import { initialState, UserAction, userReducer, User } from '../reducer/UserReducer';

interface UserContextProps {
  state: User;
  dispatch: React.Dispatch<UserAction>;
}

export const UserContext = createContext<UserContextProps | undefined>(undefined);

interface UserProviderProps {
  children: ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  const [state, dispatch] = useReducer(userReducer, initialState);

  return (
    <UserContext value={{ state, dispatch }}>
      {children}
    </UserContext>
  );
};


