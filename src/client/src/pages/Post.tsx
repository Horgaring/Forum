import * as React from 'react';
import { useParams } from 'react-router-dom';
import Header from '../components/Header';
import SideBar from '../components/SideBar';
import ContentSide from '../components/ContentSide';
import RecrentPosts from '../components/Recrent';
import PostInfo from '../components/PostInfo';


export interface IPostProps {

}

export default function Post (props: IPostProps) {
    let { id } = useParams();
    React.useEffect(() => {
        console.log(id)
        
    })
  return (
    <>
      <Header CreatePostButton={null} />

      <div className="main-container">
        <SideBar></SideBar>
        <ContentSide>
          <PostInfo PostId={id === undefined ? "" : id}></PostInfo>
        </ContentSide>
        <RecrentPosts />
      </div>
    </>
  );
}
