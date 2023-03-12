import { useRef } from 'react';
import './index.css';

const Exit = ({ onExit }) => {
	return (
		<div className="exit-container">
			<button onClick={onExit}>X</button>
		</div>
	)
}

export default Exit;