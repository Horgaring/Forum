import Header from "../components/Header";
import "../css/common/global.css";
import SideBar from "../components/SideBar";
import ContentSide from "../components/ContentSide";
import RecrentPosts from "../components/Recrent";
import PostComponent from "../components/PostComponent";
import { v4 } from "uuid";

export const Main = () => {
  return (
    <>
      <Header />

      <div className="main-container">
        <SideBar></SideBar>
        <ContentSide>
          <PostComponent
            Name="test"
            ImagePath="https://preview.redd.it/this-seems-pretty-fair-v0-rjyh77slz9wc1.png?width=640&crop=smart&auto=webp&s=41d9095fa5dc5a6697a2793d6578a75faebfe56d"
            PostId={v4()}
          ></PostComponent>
          <PostComponent
            Name="test"
            ImagePath="https://preview.redd.it/this-seems-pretty-fair-v0-rjyh77slz9wc1.png?width=640&crop=smart&auto=webp&s=41d9095fa5dc5a6697a2793d6578a75faebfe56d"
            PostId="1"
          ></PostComponent>
          <PostComponent
            Name="test"
            ImagePath="https://preview.redd.it/this-seems-pretty-fair-v0-rjyh77slz9wc1.png?width=640&crop=smart&auto=webp&s=41d9095fa5dc5a6697a2793d6578a75faebfe56d"
            PostId="1"
          ></PostComponent>
        </ContentSide>
        <RecrentPosts />
      </div>

      {/* <button onClick={signin}>sigin</button>
    <button onClick={login}>login</button>
    <button onClick={getuser}>user</button> */}
    </>
  );
};
