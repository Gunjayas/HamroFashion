import CreateUser from "./CreateUser";

export default interface UpdateUser extends CreateUser {
    id?: string;
    facebookLink?: string;
    twitterLink?: string;
    youtubeLink?: string;
    discordLink?: string;
    githubLink?: string;
}