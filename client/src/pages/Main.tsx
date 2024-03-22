import { Header } from "../components/Header"
import { Signin,Callback,GetUser } from "../services/Auth"

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
    <button onClick={signin}>sigin</button>
    <button onClick={login}>login</button>
    <button onClick={getuser}>user</button>
    </>)
}