import * as React from 'react';
import '../css/SideBar.css';
import { Link } from 'react-router-dom';

export interface ISideBarProps {
}

export default function SideBar (props: ISideBarProps) {
  const communitycontainer = (avatar: string,path: string, name: string) => (
      <Link className='community-container' to={path}>
            <img className='avatar' src={avatar}></img>
            <div className='community-name'>{name}</div>
      </Link>
  )
  return (<>
    <div className='side-bar'>
      <div className='community-list'>
        
        <li>
        {communitycontainer('https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg','/DF','Dwarf Fortres')}
        </li>
        <li>
        {communitycontainer('https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg','/DF','Dwarf Fortres')}
        </li>
        <li>
        {communitycontainer('https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg','/DF','Dwarf Fortres')}
        </li>
        <li>
        {communitycontainer('https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg','/DF','Dwarf Fortres')}
        </li>
        <li>
        {communitycontainer('https://styles.redditmedia.com/t5_2qyn1/styles/communityIcon_k33q1uumr2511.jpeg','/DF','Dwarf Fortres')}
        </li>
      </div>
    </div>
    
    </>
  );
}
