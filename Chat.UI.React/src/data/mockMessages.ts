import { Message } from '../types/Message';

// Sample data for testing UI rendering.
export const mockMessages: Message[] = [
  {
    id: '1',
    type: 'text',
    content: `**Welcome!** This is a *Markdown* message.`,
    isMine: false,
  },
  {
    id: '2',
    type: 'image',
    content: 'https://dssi.pt/wp-content/uploads/2024/12/CrashPlan-logo-Mid-Blue-800x400-1-300x150.png',
    isMine: true,
  },
  {
    id: '3',
    type: 'chart',
    content: 'chart-data-placeholder', // Will be handled by a placeholder for now
    isMine: false,
  },
  {
    id: '4',
    type: 'table',
    content: `| Name  | Age |\n|-------|-----|\n| Alice | 25  |\n| Bob   | 30  |`,
    isMine: true,
  },
  {
    id: '5',
    type: 'text',
    content: `This is a single-line message.`,
    isMine: true,
  },
  {
    id: '6',
    type: 'text',
    content: `This is a multi-line message:\nLine 1\nLine 2\nLine 3`,
    isMine: true,
  },
  {
    id: '7',
    type: 'text',
    content: `Another one:\n\n- First bullet\n- Second bullet\n\nAnd some **bold text**.`,
    isMine: false,
  },
];
