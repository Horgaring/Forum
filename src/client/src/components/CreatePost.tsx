import * as React from 'react';
import { LiaPlusSolid } from "react-icons/lia";
import '../css/CreatePost.css';
import { Link } from 'react-router-dom';

export interface Props {
}

export default function CreatePost (props: Props) {
  return (
    
    
      <Link className='create' to={'/create'}>
        <LiaPlusSolid color='white' size={20} />
        New
      </Link>
   
  );
}
