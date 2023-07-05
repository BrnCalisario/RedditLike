interface MemberDTO {
    jwt: string;
    groupId: number;
}

interface MemberRoleDTO {
    jwt: string;
    memberId: number;
    roleId: number;
    groupId: number;
}

export { MemberDTO, MemberRoleDTO };
