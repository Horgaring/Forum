import * as React from 'react';
import { useParams } from 'react-router-dom';
import { Group, GroupDetails, Post, PostCard, User  } from '../api/api';
import { v4} from 'uuid';
import Header from '../components/Header';
import SideBar from '../components/SideBar';
import ContentSide from '../components/ContentSide';
import PostComponent from '../components/PostComponent';
import RecrentPosts from '../components/Recrent';
import { UUID } from 'crypto';
import GroupInfo from '../components/GroupComponent';




export default function GroupPage () {
    const [id,setid]  = React.useState(useParams().id)
    const [catalog, setcatalog] = React.useState<PostCard[]>([])
    const [group,setgroup] = React.useState<GroupDetails>(new GroupDetails("Unknown","https://styles.redditmedia.com/t5_wyxfa/styles/communityIcon_om4k0zxbyk971.png",0,v4() as UUID,{Name: "Unknown", Id: v4() as UUID}))
    React.useEffect(() => {
        Group.Get(id as string).then(res => {
            setgroup(res)
        })
        .catch(err => {
            console.log(err)
        })
        Post.Catalog(id as string,10,1).then(res => {
          setcatalog(res)
        })
        .catch(err => {
          console.log(err)
        })
    },[])
  return (
    <>
      <Header CreatePostButton={id as string}/>
      <div className="main-container">
        <SideBar></SideBar>
        <ContentSide>
          <GroupInfo Group={group} />
          { catalog.map((x) => (
            <PostComponent
              Post={x}
              key={x.Id}
            />
          ))}
        </ContentSide>
        <RecrentPosts />
      </div>
    </>
  );
}
