interface MemberDTO {
    jwt: string;
    groupId: number;
}

interface MemberRoleDTO {
    jwt: string;
    userId: number;
    roleId: number;
    groupId: number;
}

interface MemberItem {
    id: number;
    name: string;
    role: string;
}

export { MemberDTO, MemberRoleDTO, MemberItem };
