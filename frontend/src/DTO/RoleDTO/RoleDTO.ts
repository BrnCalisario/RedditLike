interface RoleDTO {
    jwt: string,
    id: number,
    groupId: number,
    name: string,
    permissionsSet : number[]
}

export { RoleDTO }