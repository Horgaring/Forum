import axios,{AxiosRequestConfig} from 'axios'
import { UUID } from 'crypto'
import {GetAccesToken, Signin} from '../utils/Auth'
import {v4 ,validate} from 'uuid'

const url = "http://localhost:5000"  //process.env.API_URL

axios.interceptors.request.use(async function (config) {
    // Set Access Token here
    await GetAccesToken()
        .then((x)=> config.headers.Authorization = `Bearer ${x}`)
    return config;
  }, function (error) {
    return Promise.reject(error);
  });
  axios.interceptors.response.use(function (response) {
      if(response.status === 401){
          Signin()
      }
      return response;
  })
/**
 * returns an array of posts by page
 *
 * @param {Number} pagesize - The number of items per page.
 * @param {Number} pagenum - The page number to retrieve.
 * @return {Promise<PostCard[]>} A Promise that resolves to an array of PostCard objects representing the retrieved posts.
 */
export  async function  Catalog(pagesize:Number,pagenum:Number): Promise<PostCard[]> {
    const res = await axios.get(url + `/post/api/posts?PageSize=${pagesize}&PageNum=${pagenum}`)
    const groups: GroupDetails[] = [];
    for (let index = 0; index < res.data.length; index++) {
        const group = await Group.Get(res.data[index].groupId);
        groups.push(group);
    }
    return res.data.map( (x: any, index: number) => {
        return new PostCard(x.title,
            x?.user?.name ?? "",
            groups[index], 
            typeof x.content == "string" ? [x.content] : x.content, 
            x.description, 
            x.id);
    })
}
export  class Post{

    /**
     * Retrieves a post from a specific group.
     *
     * @param {UUID} Id - The unique identifier of the group.
     * @return {PostCard} A PostCard object representing the retrieved post.
     */
    public static async GetFromGroup(Id : UUID) {
        var res =  await axios.get(url + `/post/api/posts/groups/${Id}`)
        var group = await Group.Get(res.data.groupid)
        
        return new PostCard(res.data.title,
            res.data?.user.name ?? "",
            group,
            res.data.content,
            res.data.description,
            res.data.id)
    }
    /**
     * Retrieves a post with a specific Id.
     *
     * @param {UUID} Id - The unique identifier of the post.
     * @return {PostCard} A PostCard object representing the retrieved post.
     */
    public static async  Get(Id : UUID) {
        var res =  await axios.get(url + `/post/api/posts/${Id}`)
        var group = await Group.Get(res.data.groupid)
        
        return new PostCard(res.data.title,
            res.data?.user.name ?? "",
            group,
            res.data.content,
            res.data.description,
            res.data.id)
    }
    public static async   Catalog(groupid:string,pagesize:Number,pagenum:Number): Promise<PostCard[]> {
        if(!validate(groupid)) return [];
        const res = await axios.get(url + `/post/api/posts/groups/${groupid}?PageSize=${pagesize}&PageNum=${pagenum}`)
        const groups: GroupDetails[] = [];
        for (let index = 0; index < res.data.length; index++) {
            const group = await Group.Get(res.data[index].groupId);
            groups.push(group);
        }
        return res.data.map( (x: any, index: number) => {
            return new PostCard(x.title,
                x?.user?.name ?? "",
                groups[index], 
                typeof x.content == "string" ? [x.content] : x.content, 
                x.description, 
                x.id);
        })
    }
    /**
     * A method to remove a post with the specified Id.
     *
     * @param {UUID} Id - The unique identifier of the post to be removed.
     */
    public static Remove(Id : UUID) {
        axios.delete(url + `/post/api/posts?id=${Id}`)
    }
    /**
     * A description of the entire function.
     *
     * @param {UpdatePostRequest} request - The request object for updating a post.
     */
    public static Update(request : UpdatePostRequest) {
        const jsonRequest = JSON.stringify(request)
        axios.put(url + `/post/api/posts`,jsonRequest)
    }
    /**
     * Creates a new post with the provided GroupId, Title, and Description.
     *
     * @param {UUID} GroupId - The unique identifier of the group for the post.
     * @param {string} Title - The title of the new post.
     * @param {string} Description - The description of the new post.
     * @return {PostCard} A PostCard object representing the newly created post.
     */
    public static async New(GroupId:string,Title:string,Image:File | null,Description:string | null) {

        const form = new FormData()
            form.append('GroupId',GroupId)
            form.append('Title',Title)
            if(Description) form.append('Description',Description)
            if(Image) form.append('Image',Image)
        var res = await axios.post(url + '/post/api/posts',form,{
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        var group = await Group.Get(GroupId)
        
        return new PostCard(res.data.title,
            "",
            group,
            res.data.content,
            res.data.description,
            res.data.id)
    }
}
export  class  Group{
    
    public static async Get(groupId : string): Promise<GroupDetails>{
        var res = await axios.get(url + `/post/api/groups/${groupId}`)
        return new GroupDetails(
            res.data.name,
            'data:image/png;base64,' + res.data.avatar,
            res.data.followers,
            res.data.id,
            {
                Name: res.data?.owner?.name ?? "Unknown",
                Id: res.data?.owner?.id ?? "Unknown"
            }
        )
    }
    public static New(name:string,avatar:File){
        const form = new FormData()
            form.append('Name',name)
            form.append('Avatar',avatar)

        axios.post(url + `/post/api/groups`,form)
    }
    public static Update(name:string,avatar: File){
        const form = new FormData()
            form.append('Name',name)
            form.append('Avatar',avatar)
            
        axios.put(url + `/post/api/groups`,form)
    }
    public static JoinTo(groupId : UUID){
        axios.post(url + `/post/api/groups/join/${groupId}`)
    }
    public static LeaveFrom(groupId : UUID){
        axios.post(url + `/post/api/groups/join/${groupId}`)
    }
}
export class PostCard {
    /**
     * A constructor for creating a new PostCard instance.
     *
     * @param {String} name - The name of the post.
     * @param {String} userName - The username associated with the post.
     * @param {UUID} Id - The unique identifier of the post.
     * @param {GroupCard} groupCard - The group card information related to the post.
     * @param {unknown[] | unknown | undefined} contentImg - The image of the post.
     * @param {String | undefined} contentDesc - The description of the post.
     */
    constructor(
        name: String,
        userName:String,
        groupCard : GroupDetails,
        contentImg: String[] | undefined,
        contentDesc: String | undefined,
        id:UUID,
    ) {
      this.Name = name;
      this.UserName = userName
      this.Id = id
      this.GroupInfo = {
        Id: groupCard.Id,
        Name: groupCard.Name,
        Avatar: groupCard.Avatar
      }
      this.ContentImg = contentImg?.map((img) => "data:image/png;base64," + img)
      this.ConstentDesc = contentDesc
    }
    /**
    *Post Name
    */
    public Name: String
    
    public UserName: String

    public Id: UUID
    
    public GroupInfo: GroupCard
    /**
    *Constent img if there is, then display else the main text of the post differently
    */
    public ContentImg: String[] | undefined
    /**
     * Main text if ContentImg equal undefined
     */
    public ConstentDesc: String | undefined 

}
export class GroupDetails  {
    public Id : UUID
    
    public Name : String
    
    public Avatar : string
    
    public Owner : User
    
    public FollowersCount : number 

    constructor(name: String, avatarPath: string, followersCount: number, id: UUID,owner: User) {
        this.Name = name;
        this.Avatar = avatarPath;
        this.Owner = owner
        this.FollowersCount = followersCount;
        this.Id = id
    }
}
type UpdatePostRequest = {
    Id: UUID
    GroupId: UUID
    Title: String
    Description: String
}
export type User = {
    Name:string

    Id:UUID
}
export type GroupCard = {
    Id: UUID
    Name: String
    Avatar: unknown
}
