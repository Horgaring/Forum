import * as React from 'react';
import  '../css/Search.css';
import { AiOutlineSearch } from "react-icons/ai";

export interface Props {
}

export default function Search (props: Props) {
  return (
    <div className='search'>
        <AiOutlineSearch size={25}  />
      <input className='search-input' type="text" />
    </div>
  );
}
