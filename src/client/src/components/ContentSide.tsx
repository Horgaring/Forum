import * as React from 'react';
import '../css/ContentSide.css';
import PostComponent from './PostComponent';

export interface IContentSideProps {
}

export default function ContentSide (props: IContentSideProps) {
  return (
    <div className='content-container'>
      <PostComponent Name='test' ImagePath='https://preview.redd.it/this-seems-pretty-fair-v0-rjyh77slz9wc1.png?width=640&crop=smart&auto=webp&s=41d9095fa5dc5a6697a2793d6578a75faebfe56d'></PostComponent>
    </div>
  );
}
