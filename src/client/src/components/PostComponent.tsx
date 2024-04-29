import * as React from 'react';
import '../css/PostComponent.css';

export interface IPostComponentProps {
  Name: string
  ImagePath: string
}

export default function PostComponent (props: IPostComponentProps) {
  return (
    <div className='post'>
      <h1>{props.Name}</h1>
      <img src={props.ImagePath}/>
      
    </div>
  );
}
