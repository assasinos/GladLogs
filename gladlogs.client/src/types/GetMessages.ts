export interface GetAllMessages {
    messages: Message[];
}

export interface Message{

    message :string; 
    timestamp :Date;
}