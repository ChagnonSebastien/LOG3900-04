import { SocketServer } from '../socket-server';
import { ChatRoom } from './chat-room';
import { Connection } from './connection';
import { AuthenticationService } from '../user-service/authentication-service';

export class ChatService {
    private static chatService: ChatService;

    private rooms: Map<String, ChatRoom>

    private constructor() {
        this.rooms = new Map();
    }

    public static get instance(): ChatService {
        if (this.chatService === undefined) {
            this.chatService = new ChatService();
        }
        return this.chatService;
    }

    public startChatService(): void {
        this.listenForConnections();
    }

    private listenForConnections(): void {
        SocketServer.instance.on('connection', (socket: SocketIO.Socket) => {
            let connection = new Connection(socket);
            socket.on('login', (...args: any[]) => this.login(connection, args));
            socket.on('joinRoom', (...args: any[]) => this.joinRoom(connection, args));
            console.log(`New socket connection from ${socket.handshake.address}`);
        });
        
    }

    private login(connection: Connection, args: any[]): void {
        if (args.length < 2) {
            connection.socket.emit('error', 'Your request must contain the email and the password');
            return;
        }

        AuthenticationService.instance.validateCredentials(args[0], args[1]).then(valid => {
            if (!valid) {
                throw new Error('User or Password is not valid');
            }

            connection.connect(args[0]);
        }, rejectReason => {
            throw new Error(rejectReason);
        }).catch((error: Error) => {
            connection.socket.emit('error', error.message);
        })
    }

    private joinRoom(connection: Connection, args: any[]): void {
        if (args.length === 0) {
            connection.socket.emit('error', 'Your request must contain at least two parameters to connect to a room');
            return;
        }

        let room = this.rooms.get(args[0]);
        if (!room) {
            room = new ChatRoom(args[0]);
            this.rooms.set(args[0], room);
        }
        
        connection.socket.removeAllListeners('joinRoom');
        room.add(connection);
    }
}
