import * as React from 'react';
import  '../css/Search.css';
import { AiOutlineSearch } from "react-icons/ai";

export interface Props {
}

export default function Search (props: Props) {
  return (
    <div className='input-con'>
        <AiOutlineSearch size={25}  />
      <input className='input' type="text" placeholder='Search' />
    </div>
  );
}
