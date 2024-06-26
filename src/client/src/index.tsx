import React from 'react';
import ReactDOM from 'react-dom/client';
import reportWebVitals from './reportWebVitals';
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { Main } from './pages/Main';
import Post  from './pages/Post';
import GroupPage from './pages/GroupPage';
import CreatePost from './components/CreatePost';
import CreatePostPage from './pages/CreatePostPage';

const router = createBrowserRouter([
  {
    path: "/",
    element: <Main/>,
  },
  {
    path : "post/:id",
    element : <Post/>
  },
  {
    path : "/g/:id",
    element : <GroupPage/>
  },
  {
    path : "/p/create/:id",
    element : <CreatePostPage/>
  }
]);

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(

  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
