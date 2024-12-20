/**
 * Represents the standard fields that we put on all entities
 */
export default interface BaseModel {

    /**
     * If known, the Id of the user who created this model
     */
    createdById?: string;

    /**
     * The UTC date when this model was created
     */
    createdOn: Date;

    /**
     * Unique id for this user (primary id)
     */
    id: string;

    /**
     * If known, the id of the user who last updated this model
     */
    modifiedById?: string;

    /**
     * The UTC date when this model was last updated (could be null)
     */
    modifiedOn?: Date;
}