import * as React from 'react';
import '../css/PostComponent.css';
import { Link } from 'react-router-dom';
import ImageSlider from './ImageSlider';


export interface IPostComponentProps {
  Name: string
  ImagePath: string[]
  PostId: string
}

export default function PostComponent (props: IPostComponentProps) {
  
  return (
    <div className="post">
      <Link id="post-ln" to={`/post/${props.PostId}`}>
        <h1>{props.Name}</h1>
      </Link>
      <ImageSlider ImagePath={props.ImagePath}></ImageSlider>
    </div>
  );
}
