import * as React from 'react';
import { LiaPlusSolid } from "react-icons/lia";
import '../css/CreatePost.css';
import { Link } from 'react-router-dom';
import { Group } from '../api/api';

export interface Props {
    GroupID: string
}

export default function CreatePost (props: Props) {
  return (
    
    
      <Link className='create' to={`/p/create/${props.GroupID}`}>
        <LiaPlusSolid color='white' size={20} />
        New
      </Link>
   
  );
}