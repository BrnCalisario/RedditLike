import { Component, EventEmitter, Output, Input } from '@angular/core';

@Component({
	selector: 'app-vote-button',
	templateUrl: './vote-button.component.html',
	styleUrls: ['./vote-button.component.css'],
})
export class VoteButtonComponent {
	
	@Output() public OnUpvote = new EventEmitter<any>();
	@Output() public OnDownvote = new EventEmitter<any>();
	
	@Input() public votes : number = 0;

	upvoteClick = () => {
		this.OnUpvote.emit();
	}

	downvoteClick = () => {
		this.OnDownvote.emit();
	}
}
