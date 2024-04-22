import { Signin,Callback,GetUser } from "../services/Auth"
import Header from "../components/Header";
import Ico from "../assets/reddit_ico.png";
import '../css/common/global.css';
import SideBar from "../components/SideBar";

export const Main = () => {

    function signin(): void {
        Signin()
    }

    function login(): void {
        console.log(Callback())
    }

    function getuser(): void {
        GetUser().then(e => console.log(e))
    }

    return(<>
    <Header/>
    
    <SideBar/>
    
    {/* <button onClick={signin}>sigin</button>
    <button onClick={login}>login</button>
    <button onClick={getuser}>user</button> */}
    </>)
}