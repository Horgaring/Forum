import * as React from 'react';
import '../css/ContentSide.css';
import PostComponent from './PostComponent';
import { v4} from 'uuid';

export interface IContentSideProps extends React.HTMLAttributes<HTMLDivElement>  {
  
}

export default function ContentSide (props: IContentSideProps) {
  return (
    <div className='content-container'>
      {props.children}
    </div>
  );
}
