import React from 'react';

interface Props {
  url: string;
  onDelete: () => void; // Callback for delete action
}

const ImageMessage: React.FC<Props> = ({ url }) => {
  return (
    <div className="p-3 rounded bg-white shadow-sm">
      <img
        src={url}
        alt="User shared content"
        className="w-full h-auto object-contain rounded"
      />
    </div>
  );
};

export default ImageMessage;
