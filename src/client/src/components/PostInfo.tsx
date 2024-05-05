import { validate as uuidIsValid } from 'uuid';
import * as React from 'react';
import '../css/PostInfo.css';
import { Link, useNavigate  } from 'react-router-dom';
import { IoMdArrowRoundBack } from "react-icons/io";
import ImageSlider from './ImageSlider';


export interface IPostInfoProps {
  PostId: string
}

export default function PostInfo (props: IPostInfoProps) {
   
    const [postId, setPostId] = React.useState(props.PostId);
    const [author, setAuthor] = React.useState("Chebupela");
    const [group, setGroup] = React.useState("dwarf fortress");


    const nav = useNavigate();
    
      React.useEffect(() => {
        
        if (!uuidIsValid(postId)) {
            nav('/');
        }
      }, [postId])

  return (
    <>
      <div className="post-info">
        <div>
          <Link to={"/"} className="back-btn">
            <IoMdArrowRoundBack color="#fff" size={25} />
          </Link>
          <img
            src="https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg"
            className="avatar"
          ></img>
          <div>
            <div>
              <Link className="ln-credit" to={`/g/${group}`}>
                {group}
              </Link>
            </div>
            <div>
              <Link className="ln-credit" to={`/u/${author}`}>
                {author}
              </Link>
            </div>
          </div>
        </div>
        <h1>{"test"}</h1>
        <ImageSlider
          ImagePath={[
            "https://preview.redd.it/this-seems-pretty-fair-v0-rjyh77slz9wc1.png?width=640&crop=smart&auto=webp&s=41d9095fa5dc5a6697a2793d6578a75faebfe56d",
            "https://fotobase.co/files/img/photo/maikl-ili/maikl-ili-29.webp",
          ]}
        ></ImageSlider>
      </div>
      <div className="post-comments ">
        <div className="input-con comment-con">
          <input className="input" type="text" placeholder="Add a comment" />
        </div>
        <div className="comms-con">
          <div className="comm">
            <img
              src="https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg"
              className="avatar"
            ></img>
            <Link className="ln-credit" to={`/u/${author}`}>
              {author}
            </Link>
            <p>
              {
                "RimWorld is a massive open-world sandbox video game developed and published by Ludeon Studios. The game follows the player character as they explore and colonize a vast and hostile alien planet called Rimworld. The gameplay revolves around building, managing, and defending settlements, exploring the planet, and engaging in various activities such as mining, crafting, combat, and diplomacy.RimWorld offers a wide range of customization options, allowing players to create unique characters, build unique structures, and develop unique strategies to survive on Rimworld. The game features a rich and detailed world, with various biomes, terrains, and creatures to explore and interact with"
              }
            </p>
          </div>
        </div>
      </div>
    </>
  );
}

