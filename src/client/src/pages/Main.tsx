import Header from "../components/Header";
import "../css/common/global.css";
import SideBar from "../components/SideBar";
import ContentSide from "../components/ContentSide";
import RecrentPosts from "../components/Recrent";
import PostComponent from "../components/PostComponent";
import { v4 } from "uuid";
import React from "react";
import { useSearchParams } from "react-router-dom";
import { Callback, GetAccesToken, GetUser, Signin } from "../utils/Auth";
import {  Catalog, PostCard } from '../api/api';



export const Main =  () => {
  const [SearchParams] = useSearchParams();
  const [catalog, setcatalog] = React.useState<PostCard[]>([])
  React.useEffect(() => {
    if (SearchParams.get("code") !== null) {
      Callback()
        .then(() => {
          window.location.href = "/";
        })
    }
    Catalog(10, 1).then((x) => {
      setcatalog(x)
    })
    .catch((x) => {
      console.log(x)
    })
  }, [])
  const getuser = () => {
    GetUser()
      .then((x) => {
        console.log(x)
      })
  }
  return (
    <>
      <Header CreatePostButton={null} />

      <div className="main-container">
        <SideBar></SideBar>
        <ContentSide>
          { catalog.map((x) => (
            <PostComponent
              Post={x}
              key={x.Id}
            />
          ))}
          <PostComponent
            Post={new PostCard("hello", "hello", { Id: 'b56b6fea-3e1f-40c3-b825-029bbd837105', Owner: { Id: 'b56b6fea-3e1f-40c3-b825-029bbd837105', Name: 'hello' }, Name: "hello", Avatar: '', FollowersCount: 0 }, ["https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/Earth%27s_moon.jpg/697px-Earth%27s_moon.jpg","https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/Earth%27s_moon.jpg/697px-Earth%27s_moon.jpg"], "hello", 'b56b6fea-3e1f-40c3-b825-029bbd837105')}
          ></PostComponent>
    <button onClick={getuser}>user</button> 
        </ContentSide>
        <RecrentPosts />
      </div>

      
    </>
  );
};
