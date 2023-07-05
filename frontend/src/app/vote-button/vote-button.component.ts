import { Component, EventEmitter, Output, Input } from '@angular/core';

@Component({
    selector: 'app-vote-button',
    templateUrl: './vote-button.component.html',
    styleUrls: ['./vote-button.component.css'],
})
export class VoteButtonComponent {
    @Output() public OnUpvote = new EventEmitter<any>();
    @Output() public OnDownvote = new EventEmitter<any>();

    @Input() upActive : boolean = false;
    @Input() downActive : boolean = false;

    @Input() public votes: number = 0;
    @Input() public disabled : boolean = false;

    divClass = () => {
        if(this.disabled)
            return "disabled"
        return "lieks"
    }

    buttonClass = (value : boolean) => {
        if(value) 
            return "active"
        return "gray"
    }

    upvoteClick = () => {
        if(this.disabled) return
        this.OnUpvote.emit();
    };

    downvoteClick = () => {
        if(this.disabled) return
        this.OnDownvote.emit();
    };
}
