import './index.css';

const ErrorModal = ({ error }) => {
	return (
		<div className={`modal error-modal ${error && error.title ? "modal-on" : "modal-off"}`}>
			<h2>{ error.title }</h2>
			<p>{ error.message.replace("(00000000-0000-0000-0000-000000000000)", "") }</p>
		</div>
	)
}

export default ErrorModal;