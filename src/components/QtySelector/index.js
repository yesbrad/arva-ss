import './index.css';

const QtySelector = ({ value, onChange }) => {
	const Increase = () => onChange(value + 1);
	const Decrease = () => onChange(value - 1);

	return (
		<div className="qty-container">
			<button onClick={Decrease} className='qty-col decrease'>
				-
			</button>
			<div className='qty-col qty-amount'>
				{value}
			</div>
			<button onClick={Increase} className='qty-col increase'>
				+
			</button>
		</div>
	)
}

export default QtySelector;