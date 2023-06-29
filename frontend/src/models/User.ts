import { Group } from './Group';
import { Post } from './Post';

export interface User {
    id: number;
    username: string;
    email: string;
    profilePicture: number;
    groups: Group[];
    posts: Post[];
}
