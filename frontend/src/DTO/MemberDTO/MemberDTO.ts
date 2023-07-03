interface MemberDTO {
    jwt: string;
    userId: number;
    groupId: number;
}

interface MemberRoleDTO {
    jwt: string;
    memberId: number;
    roleId: number;
    groupId: number;
}

export { MemberDTO, MemberRoleDTO };
