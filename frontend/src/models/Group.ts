interface Group {
    id: number,
    name: string;
    description: string;
    ownerId: number;
    isMember: boolean;
    imageId: number | null;
    userQuantity: number;
    jwt: string;
}


interface GroupQuery {
    jwt: string;
    name: string;
}

export { Group, GroupQuery }