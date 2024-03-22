import { User, UserManager } from "oidc-client-ts";


    const  config = {
        authority: "http://localhost:5001",
        client_id: "React",
        client_secret:'ee4ecb88-4211-4a10-88e8-9276ab9d1262',
        redirect_uri: "http://localhost:3000",
        response_type: "code",
        scope: "openid profile identity-api post-api comment-api",// offline_access", 
        post_logout_redirect_uri: "http://localhost:3000/signout-oidc",
        
    }
    const  manager:UserManager = new UserManager(config);
    export const  Signin = async (): Promise<void> => {
        await manager.signinRedirect()
    }
    export const  removeUser = async (): Promise<void> => {
        await manager.removeUser()
    }
    export const  Callback = async (): Promise<User> => {
        return await manager.signinRedirectCallback()
    }
    export const GetAccesToken = async (): Promise<string | undefined> => {
        const res = await manager.getUser()
        return res?.access_token
    }
    export const GetUser = async (): Promise<User | null> =>{
        const res = await manager.getUser()
        return res;
    }
