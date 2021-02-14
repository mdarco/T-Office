import * as React from 'react';

// import './NoData.css';

function NoData() {
	return (
		<div className="py-4">
			<div className="flex items-center justify-center border-4 border-dashed border-gray-200 rounded-lg h-96">
				<img src="img/no-data-sad-face.png" alt="No data" />
			</div>
		</div>
	);
}

export default NoData;
