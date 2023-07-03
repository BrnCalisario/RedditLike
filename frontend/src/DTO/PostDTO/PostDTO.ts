interface PostDTO
{
    id: number,
    jwt: string,
    title: string,
    content: string,
    groupId: number,
    indexedImg: number,
}

interface VoteDTO
{
    jwt: string,
    value: boolean, 
    postId: number
}

interface CommentDTO
{
    id: number,
    jwt: string,
    postId: number,
    content: string
}

export { PostDTO, VoteDTO, CommentDTO }