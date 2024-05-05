import * as React from 'react';
import { MdNavigateNext,MdNavigateBefore } from "react-icons/md";

export interface IImageSliderProps {
    ImagePath: string[];

}

export default function ImageSlider (props: IImageSliderProps) {
    
  return (
    <div>
      <div style={{ position: "relative", marginTop: "0 auto" }}>
        <div className="slider">
          {props.ImagePath.map((image, index) => (
            <img className="post-image" key={index} src={image}></img>
          ))}
          {props.ImagePath.length > 1 && (
            <>
              
                <button
                  className="post-button"
                  style={{
                    left: 0,
                  }}
                  onClick={() => document.querySelector(".slider")?.scrollBy(-1, 1)}
                >
                  <MdNavigateBefore />
                </button>
              
              
                <button
                  className="post-button"
                  style={{
                    right: 0,
                  }}
                  onClick={() => document.querySelector(".slider")?.scrollBy(1, 0)}
                >
                  <MdNavigateNext />
                </button>
             
            </>
          )}
        </div>
      </div>
    </div>
  );
}
