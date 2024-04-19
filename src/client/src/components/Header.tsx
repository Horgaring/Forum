import * as React from 'react';
import  "../css/Header.css";
import Reddit from "../assets/reddit_ico.png"
import Search from './Search';
import { BiSolidUser } from "react-icons/bi";
import { BsFillChatLeftDotsFill } from "react-icons/bs";
import { IoNotifications } from "react-icons/io5";
import  CreatePost  from "./CreatePost";

export interface Props {
}

export default function Header (props: Props) {
  return (<>
    <header className='Header'>
      <nav>
        <div className='logo'>
          <img width='30px' src={Reddit}></img>
        </div>
        <div className='search-container'>
          <Search />
        </div>
        <div className='user' >
          
          <BsFillChatLeftDotsFill color='white' size={20} />
          <CreatePost />
          <IoNotifications color='white' size={20} />
          <BiSolidUser color='white' size={20} />
        </div>
      </nav>
    </header>
    </>);
}
