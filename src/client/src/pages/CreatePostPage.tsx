import * as React from 'react';
import { useParams } from 'react-router-dom';
import { Post } from '../api/api';

export interface ICreatePostPageProps {
}

export default function CreatePostPage (props: ICreatePostPageProps) {
  const [groupId, setGroupId] = React.useState<string>(useParams().id as string);
  const [title, setTitle] = React.useState('');
  const [description, setDescription] = React.useState('');
  const [file, setFile] = React.useState<File | null>(null);

  return (
    <div className="create-post-page">
      <form onSubmit={(e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('file', file!);

        Post.New(groupId, title, file, description)
          .then(() => window.location.href = "/g/" + groupId)
          .catch(console.error);
        
      }}>
        <h1>Create Post</h1>
        <input required value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Title" />
        <textarea required  value={description} onChange={(e) => setDescription(e.target.value)} placeholder="description"></textarea>
        <input  type="file" onChange={(e) => setFile(e.target.files?.[0] || null)} />
        <button type="submit">Submit</button>
      </form>
    </div>
  );
}



