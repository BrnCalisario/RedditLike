interface Group {
    id: number;
    name: string;
    description: string;
    ownerId: number;
    isMember: boolean;
    imageId: number | null;
    userQuantity: number;
    userRole: string;
    userPermissions: Permission[];
    jwt: string;
}

enum Permission {
    Post = 1,
    Delete = 2,
    EditPost = 3,
    Promote = 4,
    ManageRole = 5,
    Ban = 6,
    DropGroup = 7,
}

interface GroupQuery {
    jwt: string;
    name?: string;
    id?: number;
}

export { Group, GroupQuery, Permission };
