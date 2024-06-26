import * as React from 'react';
import  "../css/Header.css";
import Lemon from "../assets/lemon-ico.svg"
import Search from './Search';
import { BiSolidUser } from "react-icons/bi";
import { BsFillChatLeftDotsFill } from "react-icons/bs";
import { IoNotifications } from "react-icons/io5";
import { RiMenuLine } from "react-icons/ri";
import { Signin } from '../utils/Auth'
import CreatePost from './CreatePost';

export interface Props {
  CreatePostButton: null | string
}

export default function Header (props: Props) {
  
  return (
    <>
      <header className="Header">
        <nav>
          <div
            onClick={() => {
              document
                .querySelector(".side-bar")
                ?.classList.toggle("side-bar-visible");
            }}
            className="menu-btn"
          >
            <RiMenuLine size={25} color="white" />
          </div>
          <div className="logo">
            <img width="30px" src={Lemon}></img>
            <span style={{ color: "white" }}>Lemon</span>
          </div>
          <div className="search-container">
            <Search />
          </div>
          <div className="user">
            <BsFillChatLeftDotsFill color="white" size={20} />
            {props.CreatePostButton != null && <CreatePost GroupID={props.CreatePostButton as string} />}
            <IoNotifications color="white" size={20} />
            <BiSolidUser style={{ 'cursor': 'pointer'}} color="white" size={20} onClick={ async () => { await Signin()}} />
          </div>
        </nav>
      </header>
    </>
  );
}
