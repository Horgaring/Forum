import * as React from 'react';
import '../css/PostComponent.css';
import { Link } from 'react-router-dom';
import ImageSlider from './ImageSlider';
import { PostCard, Post } from '../api/api';
import { Props } from './Header';

interface IPostComponentProps {
  Post: PostCard
}


export default function PostComponent (props: IPostComponentProps) {
  const maintContent = (props.Post.ContentImg == null ? <p>{props.Post.ConstentDesc}</p> : <ImageSlider Image={props.Post.ContentImg}></ImageSlider>)
  return (
    <div className="post">
      <div className="post-info">
        <div>
          <img
            src={'data:image/png;base64,' + props.Post.GroupInfo.Avatar}
            className="avatar"
          ></img>
          <div>
            <div>
              <Link
                className="ln-credit"
                to={`/g/${props.Post.GroupInfo.Id}`}
              >
                {props.Post.GroupInfo.Name}
              </Link>
            </div>
            <div>
              <Link className="ln-credit" to={`/u/${props.Post.Id}`}>
                {props.Post.UserName}
              </Link>
            </div>
          </div>
        </div>
      </div>
      <Link id="post-ln" to={`/post/${props.Post.Id}`}>
        <h1>{props.Post.Name}</h1>
      </Link>
      {maintContent}
    </div>
  );
}
