import * as React from 'react';
import { GroupDetails } from '../api/api';

export interface IGroupInfoProps {
    Group: GroupDetails
}

export default function GroupInfo (props: IGroupInfoProps) {
  const formatfollowers = (count: number) => {
    if (count < 1000) {
      return count
    } else if (count < 1000000) {
      return (count / 1000).toFixed(1) + 'K'
    } else {
      return (count / 1000000).toFixed(1) + 'M'
    }
  }
  return (
    <div className='group-info'>
      <img src={props.Group.Avatar } ></img>
      
      <div id='group-name'  >{props.Group.Name}</div>
      <div>Followers: {formatfollowers(props.Group.FollowersCount)}</div>
    </div>
  );
}
