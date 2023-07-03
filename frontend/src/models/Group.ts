export interface Group {
    name: string;
    description: string;
    ownerId: number;
    isMember: boolean;
    imageId: number | null;
    userQuantity: number;
}
